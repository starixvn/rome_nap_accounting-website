<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerupddisableopentote">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          {lbl:disableot}
        </td>
        <td class="colW2">
          <xsl:value-of select="@disablehr" />
        </td>
        <td class="colW1">
          {lbl:isUpdateDownline}
        </td>
        <td class="colW2">
          <xsl:value-of select="@disabledownline" />
        </td>


        <td class="colW1">
          {lbl:language}
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
      </tr>

      <tr>
        <td class="colW1">
          {lbl:errCode}
        </td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1">
          {lbl:errMsg}
        </td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>

        <td class="colW1">
          {lbl:ip}
        </td>
        <td class="colW2">
          <xsl:value-of select="@ip" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:SubAccName}</td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1"></td>
        <td class="colW2">

        </td>
        <td class="colW1"></td>
        <td class="colW2">

        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
