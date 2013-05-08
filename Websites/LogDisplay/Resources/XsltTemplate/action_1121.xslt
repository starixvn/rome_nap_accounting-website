<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_updbingostatus">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          {lbl:disabledBG}
        </td>
        <td class="colW2">
          <xsl:value-of select="@disabledbg" />
        </td>
        <td class="colW1">
          {lbl:enableDownline}
        </td>
        <td class="colW2">
          <xsl:value-of select="@enabledownline" />
        </td>
        <td class="colW1">
          {lbl:syncBG}
        </td>
        <td class="colW2">
          <xsl:value-of select="@syncbg" />
        </td>
      </tr>
      <tr>
        <td class="Language">
          {lbl:language}
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
        <td class="colW1">
          {lbl:ErrCode}
        </td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>

        <td class="colW1">
          {lbl:ErrMsg}
        </td>
        <td class="colW2">
          <xsl:value-of select="@errMsg" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:IP}</td>
        <td class="colW2">
          <xsl:value-of select="@loginip" />
        </td>
        <td class="colW1">
          {lbl:SubAccName}
        </td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>

        
        <td class="colW1"></td>
        <td class="colW2">

        </td>
      </tr>
    </table>
  </xsl:template>
</xsl:stylesheet>
