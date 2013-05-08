<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

  <xsl:template match="age_customerupdcredit_ag">
    <table class="tblDesc">
      <tr>
        <td class="colW1">
          {lbl:creditAmt}
        </td>
        <td class="colW2">
          <xsl:value-of select="@creditamt" />
        </td>
        <td class="colW1">
          {lbl:language}
        </td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
        <td class="colW1">
          {lbl:creditProblem}
        </td>
        <td class="colW2">
          <xsl:value-of select="@creditproblem" />
        </td>

      </tr>

      <xsl:choose>
        <xsl:when test="$IsInternal = 0">

        </xsl:when>
        <xsl:otherwise>
          <tr>

            <td class="colW1">
              {lbl:agID}
            </td>
            <td class="colW2">
              <xsl:value-of select="@agid" />
            </td>
            <td class="colW1">
              {lbl:subAccID}
            </td>
            <td class="colW2">
              <xsl:value-of select="@subaccid" />
            </td>
            <td class="colTitle"></td>
            <td class="colValue">

            </td>
          </tr>

        </xsl:otherwise>
      </xsl:choose>
    </table>
  </xsl:template>
</xsl:stylesheet>
